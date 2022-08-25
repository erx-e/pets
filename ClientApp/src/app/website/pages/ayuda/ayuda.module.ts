import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AyudaRoutingModule } from './ayuda-routing.module';
import { AyudaComponent } from './ayuda.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PublicarComponent } from './publicar/publicar.component';


@NgModule({
  declarations: [
    AyudaComponent,
    PublicarComponent
  ],
  imports: [
    CommonModule,
    AyudaRoutingModule,
    SharedModule
  ]
})
export class AyudaModule { }
