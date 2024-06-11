import { ProductType } from "../enums/ProductType";

export type FilterCriteria = {
    categories: string[]
    activeProductType: ProductType | null
    priceRange: PriceRange
}

export type PriceRange = {
    min: number | null
    max: number | null
}