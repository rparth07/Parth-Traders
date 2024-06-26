import { ProductType } from "../enums/ProductType";

export type FilterCriteria = {
    name: string
    categories: string[]
    activeProductType: ProductType | null
    priceRange: PriceRange
    ratingRange: RatingRange
}

export type PriceRange = {
    min: number | null
    max: number | null
}

export type RatingRange = {
    min: number | null,
    max: number | null
}