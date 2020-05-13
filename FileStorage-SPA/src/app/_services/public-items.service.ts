import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PublicItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'PublicItems/files/';

  constructor(private http: HttpClient) { }

  getPublicFiles() {
    return this.http.get(this.baseUrl + this.filesUrl);
  }

  setFilePrivate(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + fileId, null);
  }

  downloadPublicFile(fileId: string): Observable<HttpEvent<Blob>> {
    return this.http.request(new HttpRequest(
      'GET',
      `${this.baseUrl}${this.filesUrl}${fileId}`,
      null,
      {
        responseType: 'blob'
      }));
  }

}
