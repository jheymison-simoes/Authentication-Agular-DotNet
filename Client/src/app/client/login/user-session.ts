export interface UserSession {
  userName: string;
  token: string;
  role: string;
  expireIn: number;
  expireTymeSpan: Date;
}
