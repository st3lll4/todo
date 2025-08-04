import { PriorityLevel } from "./PriorityLevel"

export interface ListItem {
    id?: string
    description: string
    isDone: boolean
    priority: PriorityLevel
    dueAt?: Date
    taskListId?: string
    parentItemId?: string
    parentItem?: ListItem
    subItems?: ListItem[]
}