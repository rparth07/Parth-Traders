import { ProductType } from "../enums/ProductType"

export interface Product {
    id: string,
    title: string,
    category: string,
    productType: ProductType
    sku: string,
    price: number,
    rating: number,
    colors: string[],
    sizes: string[],
    image_paths: string[]
}
