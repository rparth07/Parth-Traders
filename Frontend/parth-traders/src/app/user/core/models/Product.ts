export interface Product {
    id: string,
    title: string,
    category: string,
    sku: string,
    price: number,
    rating: number,
    colors: string[],
    sizes: string[],
    image_paths: string[]
}
