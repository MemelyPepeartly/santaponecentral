import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class YuleLogService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
