import { v4 as uuid } from 'uuid';

export default function newGuid(): string {
  return uuid();
}
