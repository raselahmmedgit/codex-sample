import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { PlaceholderComponent } from './shared/pages/placeholder.component';
import { HomeComponent } from './features/home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, title: 'Home | Elevate Commerce' },
  { path: 'products', component: PlaceholderComponent, title: 'Products' },
  { path: 'login', component: PlaceholderComponent, title: 'Login' },
  { path: 'cart', component: PlaceholderComponent, canActivate: [authGuard], title: 'Cart' },
  { path: 'orders', component: PlaceholderComponent, canActivate: [authGuard], title: 'Orders' },
  { path: '**', redirectTo: '' }
];
