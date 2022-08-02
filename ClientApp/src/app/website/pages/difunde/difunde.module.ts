import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DifundeRoutingModule } from './difunde-routing.module';
import { DifundeComponent } from './difunde.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PublicarComponent } from './publicar/publicar.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    DifundeComponent,
    PublicarComponent
  ],
  imports: [
    CommonModule,
    DifundeRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class DifundeModule { }
