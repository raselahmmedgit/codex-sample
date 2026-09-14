import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { PlaceholderComponent } from './shared/pages/placeholder.component';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { ProductListComponent } from './features/catalog/pages/product-list.component';
import { ProductDetailComponent } from './features/catalog/pages/product-detail.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, title: 'Home | Elevate Commerce' },
  { path: 'products', component: ProductListComponent, title: 'Products | Elevate Commerce' },
  { path: 'products/:id', component: ProductDetailComponent, title: 'Product details | Elevate Commerce' },
  { path: 'login', component: LoginComponent, title: 'Sign in | Elevate Commerce' },
  { path: 'register', component: RegisterComponent, title: 'Create account | Elevate Commerce' },
  { path: 'cart', component: PlaceholderComponent, canActivate: [authGuard], title: 'Cart' },
  { path: 'wishlist', component: PlaceholderComponent, canActivate: [authGuard], title: 'Wishlist' },
  { path: 'orders', component: PlaceholderComponent, canActivate: [authGuard], title: 'Orders' },
  { path: 'profile', component: PlaceholderComponent, canActivate: [authGuard], title: 'Profile' },
  { path: '**', redirectTo: '' }
];
