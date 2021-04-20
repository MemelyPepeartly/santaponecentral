import { Injectable } from '@angular/core';

export class AssignmentCSVModel
{
  Nickname!: string;
  'Real Name': string;
  Event!: string;
  Status!: string;
  'Address Line 1': string;
  'Address Line 2': string;
  City!: string;
  State!: string;
  Country!: string;
  'Postal Code': string;
  'Spaghetti Answer': string;
}

@Injectable({
  providedIn: 'root'
})
export class ExporterService {

  downloadFile(data: any, filename = 'data') {
    let csvData = this.ConvertToCSV(data, ['Nickname', 'Real Name', 'Event', 'Status', 'Address Line 1', 'Address Line 2', 'City', 'State', 'Country', 'Postal Code', 'Spaghetti Answer']);
    let blob = new Blob(['\ufeff' + csvData], { type: 'text/csv;charset=utf-8;' });
    let dwldLink = document.createElement("a");
    let url = URL.createObjectURL(blob);
    let isSafariBrowser = navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1;
    if (isSafariBrowser) {  //if Safari open in new window to save file with random filename.
      dwldLink.setAttribute("target", "_blank");
    }
    dwldLink.setAttribute("href", url);
    dwldLink.setAttribute("download", filename + ".csv");
    dwldLink.style.visibility = "hidden";
    document.body.appendChild(dwldLink);
    dwldLink.click();
    document.body.removeChild(dwldLink);
  }

  ConvertToCSV(objArray: string, headerList: string[]) {
    let array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    let str = '';
    let row = 'S.No,';

    for (let index in headerList) {
      row += headerList[index] + ',';
    }
    row = row.slice(0, -1);
    str += row + '\r\n';
    for (let i = 0; i < array.length; i++) {
      let line = (i + 1) + '';
      for (let index in headerList) {
        let head = headerList[index];

        line += ',' + array[i][head];
      }
      str += line + '\r\n';
    }
    return str;
  }
}
