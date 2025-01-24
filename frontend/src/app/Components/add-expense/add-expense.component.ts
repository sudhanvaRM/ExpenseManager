import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms'; // For template-driven forms
import { HttpClient, HttpClientModule } from '@angular/common/http'; // For HTTP requests
import { CommonModule } from '@angular/common'; // For ngFor and ngIf

@Component({
  selector: 'app-add-expense',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule],
  templateUrl: './add-expense.component.html',
  styleUrls: ['./add-expense.component.css'],
})
export class AddExpenseComponent implements OnInit {
  expense = {
    TripId: null, // Allows null for unassigned expenses
    PaidUser: '', // Set from localStorage
    Amount: null,
    Comment: '',
    Category: '',
  };

  tripList: any[] = []; // To hold trips fetched from the API
  categories: string[] = []; // To hold categories

  private tripsApiUrl = 'http://localhost:5134/api/trip/trip-details'; // API for fetching trips
  private addExpenseApiUrl = 'http://localhost:5134/api/expense/add-expense'; // API for adding expenses

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // Fetch the PaidUser from localStorage
    const userId = localStorage.getItem('user_id');
    if (userId) {
      this.expense.PaidUser = userId; // Assign the userId to the PaidUser field
      this.loadTrips(userId); // Fetch trips for the user
    } else {
      alert('User ID not found. Please log in again.');
    }

    this.loadCategories(); // Load predefined categories
  }

  // Fetch trips for the logged-in user
  loadTrips(userId: string): void {
    this.http.get(`${this.tripsApiUrl}/${userId}`).subscribe(
      (response: any) => {
        console.log('Fetched Trips:', response);
        this.tripList = [{ tripId: null, tripName: 'Unassigned' }, ...response]; // Add "Unassigned" option
        console.log('Fetched Trips:', this.tripList);
      },
      (error) => {
        console.error('Error fetching trips:', error);
        alert('Failed to fetch trips. Please try again later.');
      }
    );
  }

  // Load predefined categories
  loadCategories(): void {
    this.categories = ['Food', 'Travel', 'Accommodation', 'Entertainment'];
  }

  // Handle form submission
  onSubmit(): void {
    console.log('Submitting Expense:', this.expense);

    // Validate input fields
    if (
      this.expense.Amount === null ||
      !this.expense.Comment.trim() ||
      !this.expense.Category.trim()
    ) {
      alert('Please fill out all required fields!');
      return;
    }

    // Submit the expense to the API
    this.http.post(this.addExpenseApiUrl, this.expense).subscribe(
      (response) => {
        console.log('Expense added successfully:', response);
        alert('Expense added successfully!');
        this.resetForm();
      },
      (error) => {
        console.error('Error adding expense:', error);
        alert('Failed to add expense. Please try again.');
      }
    );
  }

  // Reset the form
  resetForm(): void {
    this.expense = {
      TripId: null, // Reset TripId to allow "Unassigned"
      PaidUser: this.expense.PaidUser, // Retain the PaidUser field
      Amount: null,
      Comment: '',
      Category: '',
    };
  }
}
