export enum ProductType {
    Buff = 'Buff',
    Machine_Tools = 'Machine Tools',
    General = 'General',
    Packing_Materials = 'Packing Materials'
}

const productTypeArray = [
    ProductType.Buff,
    ProductType.Machine_Tools,
    ProductType.General,
    ProductType.Packing_Materials
];

export function convertIntToProductType(index: number): ProductType | undefined {
    if (index >= 0 && index < productTypeArray.length) {
        return productTypeArray[index];
    }
    return undefined; // Return undefined if index is out of bounds
}