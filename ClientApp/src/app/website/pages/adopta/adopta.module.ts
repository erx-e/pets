import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdoptaRoutingModule } from './adopta-routing.module';
import { AdoptaComponent } from './adopta.component';


@NgModule({
  declarations: [
    AdoptaComponent
  ],
  imports: [
    CommonModule,
    AdoptaRoutingModule
  ]
})
export class AdoptaModule { }
