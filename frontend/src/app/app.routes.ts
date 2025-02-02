import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { AddTripComponent } from './Components/add-trip/add-trip.component';
import { AddExpenseComponent } from './Components/add-expense/add-expense.component';
import { ViewExpensesComponent } from './Components/view-expenses/view-expenses.component';
import { TripDashboardComponent } from './Components/trip-dashboard/trip-dashboard.component';
import { AddParticipantComponent } from './add-participant/add-participant.component';


export const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent },
      { path: 'add-expense', component: AddExpenseComponent},
      { path: 'add-trip', component: AddTripComponent },
      { path: 'view-expenses', component: ViewExpensesComponent},
      { path: 'view-trips',component: TripDashboardComponent },
      { path: 'add-participant/:tripId', component: AddParticipantComponent }
    ]
  },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];