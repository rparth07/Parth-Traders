export interface Product {
    id: string,
    title: string,
    type: string,
    sku: string,
    price: number,
    rating: number,
    colors: string[],
    sizes: string[],
    image_paths: string[]
}
