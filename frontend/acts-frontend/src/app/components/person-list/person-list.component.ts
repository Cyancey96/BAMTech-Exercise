import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PersonService, Person } from '../../services/person-service';

@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './person-list.component.html'
})
export class PersonListComponent implements OnInit {
  people: Person[] = [];
  newPersonName: string = '';
  loading: boolean = true;
  error: string | null = null;
  selectedPerson: Person | null = null;

  constructor(private personService: PersonService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  // Load all people
  loadPeople(): void {
    this.loading = true;
    this.personService.getAll().subscribe({
      next: (data) => {
        this.people = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to load people.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  // Add a new person
  addPerson(): void {
    if (!this.newPersonName.trim()) return;

    const newPerson: Partial<Person> = { name: this.newPersonName.trim() };

    this.personService.create(newPerson as Person).subscribe({
      next: (person) => {
        this.people.push(person);
        this.newPersonName = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to add person.';
        console.error(err);
      }
    });
  }

  viewPerson(personId: number): void {
    const found = this.people.find(p => p.personId === personId);
    this.selectedPerson = found ? { ...found } : null;
    this.cdr.detectChanges();
  }

  viewPersonByName(): void {
    if (!this.newPersonName.trim()) return;

    const found = this.people.find(p => p.name === this.newPersonName.trim());
    this.selectedPerson = found ? { ...found } : null;
    this.cdr.detectChanges();
  }

  updatePerson(): void {
    if (!this.selectedPerson) return;

    this.personService.update(this.selectedPerson.personId, this.selectedPerson).subscribe({
      next: () => {
        this.people = this.people.map(p =>
          p.personId === this.selectedPerson!.personId ? { ...this.selectedPerson! } : p
        );
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to update person.';
        console.error(err);
      }
    });
  }
}