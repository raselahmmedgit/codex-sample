import { orderStatusName } from './orders.constants';

describe('orderStatusName', () => {
  it('maps numeric API enum values to readable labels', () => {
    expect(orderStatusName(0)).toBe('Pending');
    expect(orderStatusName(5)).toBe('Delivered');
  });

  it('preserves string status values', () => {
    expect(orderStatusName('Processing')).toBe('Processing');
  });
});
