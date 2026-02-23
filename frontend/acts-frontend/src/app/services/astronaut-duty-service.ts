import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment';
import { Observable } from 'rxjs';

export interface AstronautDuty {
  dutyId: number;
  title: string;
  rank: string;
  startDate: string;
  endDate?: string;
  personId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class AstronautDutyService {
  private apiUrl = `${environment.apiUrl}/AstronautDuty`;

  constructor(private http: HttpClient) {}

  getAll(personName?: string, id?: number): Observable<AstronautDuty[]> {
    const params: string[] = [];
    if (personName) params.push(`personName=${encodeURIComponent(personName)}`);
    if (id) params.push(`id=${id}`);
    const query = params.length ? `?${params.join('&')}` : '';
    return this.http.get<AstronautDuty[]>(`${this.apiUrl}${query}`);
  }

  create(duty: AstronautDuty, personName?: string): Observable<AstronautDuty> {
    let url = this.apiUrl;
    if (personName) url += `?personName=${personName}`;
    return this.http.post<AstronautDuty>(url, duty);
  }

  update(id: number, duty: AstronautDuty): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, duty);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}