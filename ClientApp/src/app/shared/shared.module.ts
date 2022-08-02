import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostpetComponent } from './components/postpet/postpet.component';
import { PostspetComponent } from './components/postspet/postspet.component';
import { ImageComponent } from './components/image/image.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    PostpetComponent,
    PostspetComponent,
    ImageComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    PostpetComponent,
    PostspetComponent,
    ImageComponent
  ]
})
export class SharedModule { }
