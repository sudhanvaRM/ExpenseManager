import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { AddTripComponent } from './Components/add-trip/add-trip.component';

export const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent }
      // { path: 'add-trip', component: AddTripComponent }
    ]
  },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];