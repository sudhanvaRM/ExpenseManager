import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http'; // For HTTP requests
import { CommonModule } from '@angular/common'; // For ngFor and ngIf
import { FormsModule } from '@angular/forms'; // For template-driven forms

@Component({
  selector: 'app-view-expenses',
  standalone: true,
  imports: [HttpClientModule, CommonModule, FormsModule],
  templateUrl: './view-expenses.component.html',
  styleUrls: ['./view-expenses.component.css']
})
export class ViewExpensesComponent implements OnInit {
  userId: string = ''; // To hold userId from localStorage
  expenses: any[] = []; // To hold expenses fetched from the API
  tripList: any[] = []; // To hold trips fetched for the dropdown
  selectedTripId: string | null = null; // To store the selected trip for unassigned expenses

  private apiUrl = 'http://localhost:5134/api/expense/user-expenses'; // API to fetch user expenses
  private tripsApiUrl = 'http://localhost:5134/api/trip/trip-details'; // API to fetch trips

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.userId = localStorage.getItem('user_id') || ''; // Fetch userId from localStorage

    if (this.userId) {
      this.loadExpenses(this.userId); // Load expenses for the user
      this.loadTrips(this.userId); // Load trips for dropdown
    }
  }

  // Fetch user expenses based on the userId
  loadExpenses(userId: string): void {
    this.http.get(`${this.apiUrl}/${userId}`).subscribe(
      (response: any) => {
        this.expenses = response;
        console.log('Fetched expenses:', this.expenses);
      },
      (error) => {
        console.error('Error fetching expenses:', error);
        alert('Failed to fetch expenses.');
      }
    );
  }

  // Fetch trips for the dropdown
  loadTrips(userId: string): void {
    this.http.get(`${this.tripsApiUrl}/${userId}`).subscribe(
      (response: any) => {
        this.tripList = response;
        console.log('Fetched Trips:', this.tripList);
      },
      (error) => {
        console.error('Error fetching trips:', error);
        alert('Failed to fetch trips.');
      }
    );
  }

  // Assign unassigned expense to a trip
  assignToTrip(expenseId: string, tripId: string): void {
    // Call the backend API to assign the trip
    const requestBody = {
      ExpenseId: expenseId,
      TripId: tripId
    };

    console.log('Assigning trip:', requestBody);

    this.http.post('http://localhost:5134/api/expense/assign-trip', requestBody).subscribe(
      (response) => {
        alert('Expense assigned successfully!');
        this.loadExpenses(this.userId); // Refresh the expenses list
        this.selectedTripId = null;
      },
      (error) => {
        console.error('Error assigning trip:', error);
        alert('Failed to assign trip.');
      }
    );
  }
}
