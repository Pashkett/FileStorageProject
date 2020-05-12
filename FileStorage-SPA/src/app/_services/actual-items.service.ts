import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActualItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'ActualItems/files/';

  constructor(private http: HttpClient) { }

  getActualFiles() {
    return this.http.get(this.baseUrl + 'ActualItems/files/');
  }

  downloadActualFile(fileId: string): Observable<HttpEvent<Blob>> {
    return this.http.request(new HttpRequest(
      'GET',
      `${this.baseUrl}${this.filesUrl}${fileId}`,
      null,
      {
        responseType: 'blob'
      }));
  }

  moveToRecycleBin(fileId: string) {
    return this.http.delete(this.baseUrl + this.filesUrl + fileId);
  }

}
