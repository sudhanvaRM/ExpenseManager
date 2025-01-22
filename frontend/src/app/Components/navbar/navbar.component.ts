import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(private router: Router) {}

  handleLogin(){
    this.router.navigate(['/home/login']);
  }

  handleAddTrip(){
    this.router.navigate(['/home/add-trip']);
  }

  handleAddExpense(){
    this.router.navigate(['/home/add-expense']);
  }

  handleViewExpenses(){
    this.router.navigate(['/home/view-expenses']);
  }

}
