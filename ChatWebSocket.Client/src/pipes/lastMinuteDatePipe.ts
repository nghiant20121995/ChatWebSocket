import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'LastMinuteDate',
  pure: true,
})
export class LastMinuteDatePipe implements PipeTransform {
  transform(input: Date | undefined) {
    if (!input) {
      return '';
    }
    let currentDate = new Date();
    let inputDate = new Date(input);
    let timeDiff = Math.abs(currentDate.getTime() - inputDate.getTime());
    let totalSeconds = Math.floor(timeDiff / 1000);
    if (totalSeconds < 60) return `${this.transformNumber(totalSeconds)} sec`;
    let totalMinutes = Math.floor(timeDiff / (1000 * 60));
    if (totalMinutes < 60) return `${this.transformNumber(totalMinutes)} min`;
    let totalHours = Math.floor(timeDiff / (1000 * 60 * 60));
    if (totalHours < 24) return `${this.transformNumber(totalHours)} hr`;
    let totalDays = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
    if (totalDays < 30) return `${this.transformNumber(totalDays)} day`;
    let totalWeeks = Math.floor(timeDiff / (1000 * 60 * 60 * 24 * 7));
    if (totalWeeks < 4) return `${this.transformNumber(totalWeeks)} week`;
    let totalMonths = Math.floor(timeDiff / (1000 * 60 * 60 * 24 * 30));
    if (totalMonths < 12) return `${this.transformNumber(totalMonths)} month`;
    let totalYears = Math.floor(timeDiff / (1000 * 60 * 60 * 24 * 365));
    if (totalYears < 2) return `${this.transformNumber(totalYears)} year`;
    return '';
  }

  transformNumber(input: number) {
    if (input < 10) {
      return '0' + input;
    }
    return input;
  }
}