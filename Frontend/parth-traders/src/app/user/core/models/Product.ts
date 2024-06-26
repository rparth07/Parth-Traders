import { ProductType } from "../enums/ProductType"

export interface Product {
    id: string,
    productName: string,
    productDescription: string;
    productType: ProductType
    category: string,
    price: number,
    discount: number,
    unitsInStock: number,
    sku: string,
    rating: number,
    sizes: string[],
    image_paths: string[]
}