import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostpetComponent } from './components/postpet/postpet.component';
import { FilterComponent } from './components/filter/filter.component';
import { PostspetComponent } from './components/postspet/postspet.component';
import { ImageComponent } from './components/image/image.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { PublishComponent } from './components/publicar/publish.component';



@NgModule({
  declarations: [
    PostpetComponent,
    PostspetComponent,
    ImageComponent,
    FilterComponent,
    SpinnerComponent,
    PublishComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule
  ],
  exports: [
    PostpetComponent,
    PostspetComponent,
    ImageComponent,
    FilterComponent,
    SpinnerComponent,
    PublishComponent
  ]
})
export class SharedModule { }
