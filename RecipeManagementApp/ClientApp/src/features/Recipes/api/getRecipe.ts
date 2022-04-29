import { api } from '@/lib/axios';
import { AxiosResponse } from 'axios';
import { useQuery } from 'react-query';
import { RecipeDto } from '../types';
import { RecipeKeys } from './recipe.keys';

export const getRecipe = (id: string) => {
	return api
		.get(`/api/recipes/${id}`)
		.then((response: AxiosResponse<RecipeDto>) => response.data);
};

export const useGetRecipe = (id: string) => {
	return useQuery(RecipeKeys.detail(id), () => getRecipe(id));
};
