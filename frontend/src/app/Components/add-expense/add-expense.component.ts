import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Import FormsModule for template-driven forms
import { HttpClient, HttpClientModule } from '@angular/common/http'; // Import HttpClient for HTTP requests
import { CommonModule } from '@angular/common'; // Import CommonModule for ngFor and ngIf

@Component({
  selector: 'app-add-expense',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule], // Ensure CommonModule is included
  templateUrl: './add-expense.component.html',
  styleUrls: ['./add-expense.component.css']
})
export class AddExpenseComponent implements OnInit {
  expense = {
    TripId: null,
    PaidUser: '',  // This will be populated from localStorage
    Amount: null,
    Comment: '',
    Category: '',
  };

  tripList: any[] = [];
  categories: string[] = [];

  private apiUrl = 'http://localhost:5134/api/expense/add-expense'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // Fetch the PaidUser from localStorage and assign it
    const userId = localStorage.getItem('user_id'); // Fetch from localStorage
    if (userId) {
      this.expense.PaidUser = userId; // Set PaidUser if a user_id exists in localStorage
    }

    // Load data for the dropdowns
    this.loadTripData();
    this.loadCategoryData();
  }

  // Load dummy trip data
  loadTripData(): void {
    this.tripList = [
      { id: '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', name: 'Trip to Paris' },
      { id: '341c1c4f-0b60-4b56-a741-cd3a703f8082', name: 'Weekend at the Beach' },
      { id: '755b3cda-d591-4f3c-9d2c-f73cb51d4d39', name: 'Hiking in the Mountains' },
    ];
    console.log('Trip List:', this.tripList); // Debugging
  }

  // Load dummy categories
  loadCategoryData(): void {
    this.categories = ['Food', 'Travel', 'Accommodation', 'Entertainment'];
    console.log('Categories:', this.categories); // Debugging
  }

  onSubmit(): void {
    console.log('Expense Data:', this.expense); // Debugging: Log the data inputted by the user

    // Check if any of the required fields are missing or empty
    if (
      !this.expense.TripId ||
      !this.expense.Amount ||
      !this.expense.Comment.trim() ||
      !this.expense.Category.trim()
    ) {
      alert('Please fill out all required fields!');
      return;
    }

    // Proceed with the API request
    this.http.post(this.apiUrl, this.expense).subscribe(
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

  resetForm(): void {
    this.expense = {
      TripId: null,
      PaidUser: '', // Clear the PaidUser field when resetting the form
      Amount: null,
      Comment: '',
      Category: '',
    };
  }
}
