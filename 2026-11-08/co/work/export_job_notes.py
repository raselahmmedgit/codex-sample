import os
import re
import zipfile
import xml.etree.ElementTree as ET


SOURCE = r"C:\Users\rasel.ahmmed\OneDrive - BRAC IT Services Limited\Project Files\Job-Personal-Notes.xlsx"
OUTPUT_DIR = r"C:\Users\rasel.ahmmed\Documents\Job-Personal-Notes_Text"
NS = {"m": "http://schemas.openxmlformats.org/spreadsheetml/2006/main"}
REL_NS = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"


def col_number(cell_ref: str) -> int:
    letters = re.match(r"[A-Z]+", cell_ref).group(0)
    result = 0
    for letter in letters:
        result = result * 26 + ord(letter) - 64
    return result


def row_number(cell_ref: str) -> int:
    return int(re.search(r"\d+", cell_ref).group(0))


def text_from_rich_node(node) -> str:
    return "".join((t.text or "") for t in node.iter("{%s}t" % NS["m"]))


def sanitize_text(value: str) -> str:
    # Keep one output record per spreadsheet row while retaining embedded formatting.
    return value.replace("\t", r"\t").replace("\r\n", r"\n").replace("\n", r"\n").replace("\r", r"\n")


def cell_value(cell, shared_strings):
    formula = cell.find("m:f", NS)
    if formula is not None:
        return "=" + (formula.text or "")

    inline = cell.find("m:is", NS)
    if inline is not None:
        return sanitize_text(text_from_rich_node(inline))

    value = cell.find("m:v", NS)
    if value is None:
        return ""

    raw = value.text or ""
    if cell.attrib.get("t") == "s":
        try:
            return sanitize_text(shared_strings[int(raw)])
        except (ValueError, IndexError):
            return raw
    if cell.attrib.get("t") == "b":
        return "TRUE" if raw == "1" else "FALSE"
    if cell.attrib.get("t") == "e":
        return raw
    return sanitize_text(raw)


def main():
    os.makedirs(OUTPUT_DIR, exist_ok=True)
    with zipfile.ZipFile(SOURCE) as archive:
        shared_strings = []
        if "xl/sharedStrings.xml" in archive.namelist():
            root = ET.fromstring(archive.read("xl/sharedStrings.xml"))
            shared_strings = [text_from_rich_node(si) for si in root.findall("m:si", NS)]

        workbook = ET.fromstring(archive.read("xl/workbook.xml"))
        rels = ET.fromstring(archive.read("xl/_rels/workbook.xml.rels"))
        rel_map = {rel.attrib["Id"]: rel.attrib["Target"] for rel in rels}
        report = []

        for sheet in workbook.find("m:sheets", NS):
            name = sheet.attrib["name"]
            rel_id = sheet.attrib["{%s}id" % REL_NS]
            target = rel_map[rel_id]
            if not target.startswith("xl/"):
                target = "xl/" + target.lstrip("/")
            root = ET.fromstring(archive.read(target))

            rows = {}
            max_row = 0
            max_col = 0
            for cell in root.findall(".//m:c", NS):
                ref = cell.attrib["r"]
                row = row_number(ref)
                col = col_number(ref)
                rows.setdefault(row, {})[col] = cell_value(cell, shared_strings)
                max_row = max(max_row, row)
                max_col = max(max_col, col)

            path = os.path.join(OUTPUT_DIR, name + ".txt")
            with open(path, "w", encoding="utf-8", newline="\n") as handle:
                for row_number_value in range(1, max_row + 1):
                    row = rows.get(row_number_value, {})
                    last_col = max(row.keys(), default=0)
                    handle.write("\t".join(row.get(col, "") for col in range(1, last_col + 1)))
                    handle.write("\n")

            report.append((name, max_row, max_col, sum(len(row) for row in rows.values()), path))

    print("OUTPUT_DIR", OUTPUT_DIR)
    for name, max_row, max_col, cell_count, path in report:
        print(f"{name}\trows={max_row}\tmax_cols={max_col}\tnonempty_cells={cell_count}\t{path}")


if __name__ == "__main__":
    main()
