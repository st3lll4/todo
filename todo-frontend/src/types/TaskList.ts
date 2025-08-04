import { ListItem } from "./ListItem"

export interface TaskList {
    id?: string
    title: string
    listItems?: ListItem[]
}