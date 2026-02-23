import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment';
import { Observable } from 'rxjs';

export interface Person {
  personId: number;
  name: string;
  createdAt: string;
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private apiUrl = `${environment.apiUrl}/Person`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Person[]> {
    return this.http.get<Person[]>(this.apiUrl);
  }

  getByIdOrName(id?: number, name?: string): Observable<Person[]> {
    const params: string[] = [];
    if (id) params.push(`id=${id}`);
    if (name) params.push(`name=${encodeURIComponent(name)}`);
    const query = params.length ? `?${params.join('&')}` : '';
    return this.http.get<Person[]>(`${this.apiUrl}${query}`);
  }

  create(person: Person): Observable<Person> {
    return this.http.post<Person>(this.apiUrl, person);
  }

  update(person: Person, id?: number, name?: string): Observable<void> {
    const params: string[] = [];
    if (id) params.push(`id=${id}`);
    if (name) params.push(`name=${encodeURIComponent(name)}`);
    const query = params.length ? `?${params.join('&')}` : '';
    return this.http.put<void>(`${this.apiUrl}${query}`, person);
  }

  delete(id?: number, name?: string): Observable<void> {
    const params: string[] = [];
    if (id) params.push(`id=${id}`);
    if (name) params.push(`name=${encodeURIComponent(name)}`);
    const query = params.length ? `?${params.join('&')}` : '';
    return this.http.delete<void>(`${this.apiUrl}${query}`);
  }
}