import { Routes } from '@angular/router';
import { StarshipsPageComponent } from './pages/starships-page/starships-page.component';
import { BookShipmentPageComponent } from './pages/book-shipment-page/book-shipment-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/starships',
    pathMatch: 'full'
  },
  {
    path: 'starships',
    component: StarshipsPageComponent
  },
  {
    path: 'book-shipment',
    component: BookShipmentPageComponent
  },
  {
    path: '**',
    redirectTo: '/starships'
  }
];
