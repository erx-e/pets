import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EncuentraRoutingModule } from './encuentra-routing.module';
import { EncuentraComponent } from './encuentra.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PublicarComponent } from './publicar/publicar.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    EncuentraComponent,
    PublicarComponent
  ],
  imports: [
    CommonModule,
    EncuentraRoutingModule,
    ReactiveFormsModule,
    SharedModule,
  ]
})
export class EncuentraModule { }
