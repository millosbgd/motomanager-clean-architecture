import { Routes } from '@angular/router';
import { VehiclesComponent } from './vehicles/vehicles.component';
import { ClientsComponent } from './clients/clients.component';
import { ServiceOrdersComponent } from './service-orders/service-orders.component';
import { MaterialsComponent } from './materials/materials.component';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/vehicles', pathMatch: 'full' },
  { path: 'vehicles', component: VehiclesComponent, canActivate: [authGuard] },
  { path: 'clients', component: ClientsComponent, canActivate: [authGuard] },
  { path: 'service-orders', component: ServiceOrdersComponent, canActivate: [authGuard] },
  { path: 'materials', component: MaterialsComponent, canActivate: [authGuard] }
];
