import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AstronautDutyService, AstronautDuty } from '../../services/astronaut-duty-service';

@Component({
  selector: 'app-astronaut-duty',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './astronaut-duty.component.html'
})
export class AstronautDutyComponent {
  // View duties
  astronautName: string = '';
  searchedAstronautName: string = '';
  duties: AstronautDuty[] = [];

  // Create duty
  showCreateForm: boolean = false;
  newDuty: Partial<AstronautDuty> = {};
  newDutyPersonName: string = '';

  loading: boolean = false;
  error: string | null = null;
  successMessage: string | null = null;
  hasSearched: boolean = false;

  constructor(
    private dutyService: AstronautDutyService,
    private cdr: ChangeDetectorRef
  ) {}

  createDuty(): void {
    this.hasSearched = false;
    this.showCreateForm = true;
    this.duties = [];
    this.error = null;
    this.successMessage = null;
    this.newDuty = {};
    this.cdr.detectChanges();
  }

  submitNewDuty(): void {
    if (!this.newDuty.title || !this.newDuty.rank || !this.newDuty.startDate) {
      this.error = 'Title, rank, and start date are required.';
      return;
    }

    this.loading = true;
    this.dutyService.create(this.newDuty as AstronautDuty, this.newDutyPersonName || undefined).subscribe({
      next: (duty) => {
        this.successMessage = `Duty "${duty.title}" created successfully.`;
        this.showCreateForm = false;
        this.newDuty = {};
        this.newDutyPersonName = '';
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to create duty.';
        console.error(err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  viewDuties(): void {
    if (!this.astronautName.trim()) {
      this.error = 'Please enter an astronaut name.';
      return;
    }

    this.searchedAstronautName = this.astronautName;
    this.hasSearched = true;
    this.showCreateForm = false;
    this.loading = true;
    this.error = null;
    this.successMessage = null;

    this.dutyService.getAll(this.astronautName.trim()).subscribe({
      next: (data) => {
        this.duties = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.duties = [];
        this.error = 'Failed to load duties.';
        console.error(err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  cancelCreate(): void {
    this.showCreateForm = false;
    this.newDuty = {};
    this.error = null;
    this.cdr.detectChanges();
  }
}