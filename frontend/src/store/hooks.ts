// יכיל Hooks מותאמים אישית שיעזרו לרכיבי React לגשת למידע שב־Store.

// store/hooks.ts
import { useDispatch, useSelector } from 'react-redux';
import type { RootState, AppDispatch } from './store';

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();