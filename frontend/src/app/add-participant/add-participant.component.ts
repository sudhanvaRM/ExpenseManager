import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-participant',
  templateUrl: './add-participant.component.html',
  styleUrls: ['./add-participant.component.css'],
  imports: [CommonModule, FormsModule],
})
export class AddParticipantComponent implements OnInit {
  tripId: string = '';
  participantsNotInTrip: any[] = [];
  selectedUserId: string = '';
  isLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.tripId = this.route.snapshot.paramMap.get('tripId') || '';
    this.fetchParticipantsNotInTrip(this.tripId);
  }

  // Fetch participants not in the trip
  fetchParticipantsNotInTrip(tripId: string): void {
    this.isLoading = true;
    this.http.get(`http://localhost:5134/api/expense/NotInTrip/${tripId}`).subscribe(
      (data: any) => {
        this.participantsNotInTrip = data;
        console.log('Participants not in trip:', this.participantsNotInTrip);
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching participants not in trip', error);
        this.isLoading = false;
      }
    );
  }

  // Add participant to the trip
  addParticipant(): void {
    if (!this.selectedUserId || !this.tripId) {
      alert('Please select a participant to add.');
      return;
    }

    const addTripPayload = {
      TripId: this.tripId,
      UserId: this.selectedUserId,
    };

    this.isLoading = true;
    this.http.post('http://localhost:5134/api/expense/add-person-to-trip', addTripPayload).subscribe(
      (response: any) => {
        alert('Participant added successfully!');
        // this.router.navigate(['/']); // Navigate back to the trip dashboard
        this.isLoading = false;
      },
      (error) => {
        console.error('Error adding participant to trip', error);
        alert('Failed to add participant. Please try again.');
        this.isLoading = false;
      }
    );
  }

  // Navigate back to the Trip Dashboard when Close button is clicked
  goBackToDashboard(): void {
    this.router.navigate(['/home/view-trips']); // Route back to the trip dashboard
  }
}
