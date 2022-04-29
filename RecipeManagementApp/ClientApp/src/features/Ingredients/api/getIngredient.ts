import { api } from '@/lib/axios';
import { AxiosResponse } from 'axios';
import { useQuery } from 'react-query';
import { IngredientDto } from '../types';
import { IngredientKeys } from './ingredient.keys';

export const getIngredient = (id: string) => {
	return api
		.get(`/api/ingredients/${id}`)
		.then((response: AxiosResponse<IngredientDto>) => response.data);
};

export const useGetIngredient = (id: string) => {
	return useQuery(IngredientKeys.detail(id), () => getIngredient(id));
};
