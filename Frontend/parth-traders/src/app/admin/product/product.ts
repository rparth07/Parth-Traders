export interface Product {
  productName: string;
  productType: ProductType;
  productDescription: string;
  supplierName: string;
  categoryName: string;
  piecesPerUnit: number;
  singlePieceMRP: number;
  unitPrice: number;
  discount: number;
  unitsInStock: number;
}

export enum ProductType {
  Buff,
  Machine_tools,
  General,
  Packing_materials,
}
