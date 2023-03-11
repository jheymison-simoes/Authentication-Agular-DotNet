import {FormGroup} from '@angular/forms'
// export {}
declare module '@angular/forms' {
  interface FormGroup {
    containError(this: FormGroup, propertyName: string, propertyError: string): boolean;
    checkTouchedOrDirty(this: FormGroup, propertyName: string): boolean;
    getMessageError(this: FormGroup, validationMessages: any, propertyName: string, propertyError: string): string;
  }
}

FormGroup.prototype.containError = function (
  this: FormGroup,
  propertyName: string,
  propertyError: string
): boolean {
  return this.get(propertyName)?.errors?.[propertyError] ?? false;
}

FormGroup.prototype.checkTouchedOrDirty = function (
  this: FormGroup,
  propertyName: string
): boolean {
  return (this.get(propertyName)?.dirty || this.get(propertyName)?.touched) ?? false;
}

FormGroup.prototype.getMessageError = function (
  this: FormGroup,
  validationMessages: any,
  propertyName: string,
  propertyError: string
): string {
  return validationMessages[propertyName][propertyError];;
}


// FormGroup.prototype.getError = function (this: FormGroup, propertyName: string, propertyError: string): boolean {
//   return this.get(propertyName)?.errors?.[propertyError] ?? false;
// }


// interface FormGroup {
//   containError(this: FormGroup, propertyName: string, propertyError: string): boolean;
//   checkTouchedOrDirty(this: FormGroup, propertyName: string): boolean;
//   getError(this: FormGroup, propertyError: string): string;
// }

// public static formGetErrors(this: FormGroup, propertyName: string, propertyError: string): boolean {
//   return this.get(propertyName)?.errors?.[propertyError] ?? false;
// }

// public static formCheckTouchedOrDirty(this: FormGroup, propertyName: string): boolean{
//   return (this.get(propertyName)?.dirty || this.get(propertyName)?.touched) ?? false;
// }

// formGetMessageErrors(propertyName: string, propertyError: string): string {
//   let result = this.validationMessages[propertyName][propertyError];
//   return result;
// }
