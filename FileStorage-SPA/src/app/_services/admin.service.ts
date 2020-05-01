import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = 'https://localhost:5001/api/admin/usersWithRoles/';

constructor(private http: HttpClient) { }

getUsersWithRoles() {
  return this.http.get(this.baseUrl);
}

}
