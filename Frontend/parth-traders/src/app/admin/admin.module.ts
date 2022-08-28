import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { EnumConverterPipe } from './shared/enum-converter.pipe';

@NgModule({
  declarations: [EnumConverterPipe],
  imports: [CommonModule, NgbModule],
  exports: [EnumConverterPipe],
})
export class AdminModule {}
