import { Pipe, PipeTransform } from '@angular/core';
import { ProductType } from '../product/product';

@Pipe({
  name: 'enumConverter',
})
export class EnumConverterPipe implements PipeTransform {
  transform(value: number): string {
    return ProductType[value];
  }
}
