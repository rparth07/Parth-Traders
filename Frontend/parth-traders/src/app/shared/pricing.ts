// Round the discounted unit price once, then multiply by quantity (matching PayPal).
export function discountedUnitPrice(price: number, percentage: number): number {
  return Math.round((price * (100 - percentage) / 100 + Number.EPSILON) * 100) / 100;
}
