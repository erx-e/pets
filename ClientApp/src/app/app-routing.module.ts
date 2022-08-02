import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { QuicklinkStrategy } from 'ngx-quicklink'
import { NotfoundComponent } from './not-found/notfound.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./website/website.module').then((m) => m.WebsiteModule),
    // data: {
    //   preload: true,
    // },
  },

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {preloadingStrategy: QuicklinkStrategy}),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
