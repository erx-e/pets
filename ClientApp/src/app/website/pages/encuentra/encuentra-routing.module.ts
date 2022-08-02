import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EncuentraComponent } from "./encuentra.component";
import { PublicarComponent } from "./publicar/publicar.component";

const routes: Routes = [
  {
    path: "",
    component: EncuentraComponent,
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
  exports: [RouterModule],
})
export class EncuentraRoutingModule {}
