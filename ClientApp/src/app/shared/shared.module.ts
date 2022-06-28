import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostpetComponent } from './components/postpet/postpet.component';
import { PostspetComponent } from './components/postspet/postspet.component';



@NgModule({
  declarations: [
    PostpetComponent,
    PostspetComponent
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
