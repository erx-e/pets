import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AyudaComponent } from '../ayuda/ayuda.component';
import { PublicarComponent } from './publicar/publicar.component';

const routes: Routes = [
  {
    path: "",
    component: AyudaComponent,
  },
  {
    path: "publicar",
    component: PublicarComponent,
  },
  {
    path: "editar/:id",
    component: PublicarComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdoptaRoutingModule { }
