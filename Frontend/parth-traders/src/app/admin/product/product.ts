export interface Product {
  position: number;
  productSku: string;
  productName: string;
  productType: ProductType;
  productDescription: string;
  productRating: number;
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
