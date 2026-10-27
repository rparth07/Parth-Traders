import { ProductType } from "../enums/ProductType"

export interface Product {
    productId: number,
    productSku: string,
    productName: string,
    productDescription: string;
    productRating: number,
    productType: ProductType | number,
    categoryName: string,
    supplierName: string,
    piecesPerUnit: number,
    singlePieceMRP: number,
    unitPrice: number,
    discount: number,
    unitsInStock: number,
    image_paths: string[]
}