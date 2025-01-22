import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { AddTripComponent } from './Components/add-trip/add-trip.component';
import { AddExpenseComponent } from './Components/add-expense/add-expense.component';
import { ViewExpensesComponent } from './Components/view-expenses/view-expenses.component';

export const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent },
      { path: 'add-expense', component: AddExpenseComponent},
      { path: 'add-trip', component: AddTripComponent },
      { path: 'view-expenses', component: ViewExpensesComponent}
    ]
  },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];