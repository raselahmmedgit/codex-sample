import { Component, input } from '@angular/core';

@Component({ selector: 'app-placeholder', standalone: true, template: '<main class="container py-5"><h1>{{ title() }}</h1><p>Feature implementation is ready for the next frontend phase.</p></main>' })
export class PlaceholderComponent {
  readonly title = input('E-Commerce');
}
